local function get_module(text, diffs)
    local pattern = "--| @module%s+([%a_]+)%s+\n"
    local module_name = text:match(pattern)
    
    if module_name then
        print("Module:", module_name)
    else
        return "", nil
    end
    

    diffs[#diffs+1] = {
        start  = 1,
        finish = 0,
        text   = ('%s\n%s = {}'):format("---@diagnostic disable-next-line: lowercase-global", module_name),
    }

    return module_name, diffs
end

local function get_variables(text, diffs, module, variables)
    local pattern = "\n()([%a_%d]+)()%s+="

    for start, name, finish in text:gmatch(pattern) do
        --print(("start: %s, variable: %s, finish: %s"):format(start, name, finish))
        variables[#variables+1] = {name = name, start = start}

        diffs[#diffs+1] = {
            start  = start,
            finish = finish-1,
            text   = ('%s.%s '):format(module, name),
        }
    end

    return variables, diffs
end

local function replace_variables(text, diffs, module, variables)
    for _, variable in pairs(variables) do
        local pattern = "[^a%.:]()" .. variable.name .. "()[^%a_%d]"

        for start, finish in text:gmatch(pattern) do
            if variable.start ~= start then
                --print(("replace: start: %s, variable: %s, finish: %s"):format(start, variable, finish))

                diffs[#diffs+1] = {
                    start  = start,
                    finish = finish-1,
                    text   = ('%s.%s'):format(module, variable.name),
                }
            end
        end
    end

    return diffs
end

local function get_functions(text, diffs, module, functions)
    local pattern = "\nfunction%s+()([%a_%d]+)()%("

    for start, name, finish in text:gmatch(pattern) do
        --print(("start: %s, function: %s, finish: %s"):format(start, name, finish))
            functions[#functions+1] = {name = name, start = start}

        diffs[#diffs+1] = {
            start  = start,
            finish = finish-1,
            text   = ('%s.%s '):format(module, name),
        }
    end

    return functions, diffs
end

local function replace_functions(text, diffs, module, functions)
    for _, func in pairs(functions) do
        local pattern = "[^a%.:]()" .. func.name .. "()[^%a_%d]"

        for start, finish in text:gmatch(pattern) do
            if func.start ~= start then
                --print(("replace: start: %s, func: %s, finish: %s"):format(start, func, finish))

                diffs[#diffs+1] = {
                    start  = start,
                    finish = finish-1,
                    text   = ('%s.%s'):format(module, func.name),
                }
            end
        end
    end

    return diffs
end

local function replace_class_definitions(text, diffs)
    -- "\n()" captures (start) the position of the start of the match, considering a newline character before the 'class' keyword.
    -- "%s*" matches any whitespace characters (including none) between the start and the 'class' keyword.
    -- "class" is a literal match for the keyword 'class'.
    -- "%s+" matches one or more whitespace characters between 'class' and the class name.
    -- "\"([%a_%d]+)\"" captures (name) the class name enclosed in double quotes, allowing alphanumeric characters and underscores.
    -- "%s*" allows for optional whitespace after the class name.
    -- "%(%?([%a_%d]*)%]?)?" captures (optionalParent) an optional parent class name within parentheses, allowing for an optional closing bracket.
    -- "()" captures (finish) the position after the entire match.
    local pattern = "\n()class%s+\"([%a_%d]+)\"%s%(?([%a_%d]*)%]?%)?()"

    for start, name, optionalParent, finish in text:gmatch(pattern) do
        print(("class declaration: start: %s, class: %s, optionalParent: %s, finish: %s"):format(start, name, optionalParent, finish))

        local decorator = "--- @class " .. name

        if optionalParent:len() > 0 then
            decorator = decorator .. ":" .. optionalParent
        end

        diffs[#diffs+1] = {
            start  = start,
            finish = finish-1,
            text   = ('%s\n%s = {}\n'):format(decorator, name),
        }
    end

    return diffs
end

local constructor_shema = [[
--- @return %s
function %s.%s(%s)
    --- @as %s
    local result = {}

    %s.__init(result%s)

    return result
end

]]

local function replace_class_constructors(text, diffs, module)
    local pattern = "\n()function%s+([%a_%d]+):__init%(([^\n%)]*)%)()%ss?u?p?e?r?%(?[^\n%)]*%)?()"

    for start, name, arguments, argumentFinish, finish in text:gmatch(pattern) do
        print(("class constructor: start: %s, class: %s, arguments: [%s], argumentFinish: %s, finish: %s"):format(start, name, arguments, argumentFinish, finish))

        local construcor_args = ""

        if arguments:len() > 0 then
            construcor_args = ", " .. arguments
        end

        diffs[#diffs+1] = {
            start  = start,
            finish = start-1,
            text   = constructor_shema:format(name, module, name, arguments, name, name, construcor_args),
        }

        -- remove super()
        diffs[#diffs+1] = {
            start  = argumentFinish,
            finish = finish-1,
            text   = ('\n'),
        }
    end

    return diffs
end

function OnSetText(uri, text)
    local module = ""
    local diffs = {}
    local variables = {}
    local functions = {}

    module, diffs = get_module(text, diffs)
    
    if diffs == nil then
        return nil
    end

    variables, diffs = get_variables(text, diffs, module, variables)
    diffs = replace_variables(text, diffs, module, variables)

    functions, diffs = get_functions(text, diffs, module, functions)
    diffs = replace_functions(text, diffs, module, functions)
    diffs = replace_class_definitions(text, diffs)
    diffs = replace_class_constructors(text, diffs, module)

    return diffs
end