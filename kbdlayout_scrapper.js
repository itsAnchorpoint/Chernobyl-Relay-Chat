const get_keys = (kr) => [...document.querySelectorAll(kr + ">.key")].map(key => [...key.querySelectorAll('.kl')]).map(keys => {
    if (keys.length == 0) {
        return []
    }


    return keys.map(key => key.innerText)
})

const keys = [
    get_keys('.kr2'),
    get_keys('.kr3'),
    get_keys('.kr4'),
    get_keys('.kr5'),
]

const result = `
    ["lang"] = {
        [2]  = { [false] = '${keys[0][1][1]}',  [true] = "${keys[0][1][0]}", }, -- [${keys[0][1].join(", ")}]
        [3]  = { [false] = '${keys[0][2][1]}',  [true] = "${keys[0][2][0]}", }, -- [${keys[0][2].join(", ")}]
        [4]  = { [false] = '${keys[0][3][1]}',  [true] = "${keys[0][3][0]}", }, -- [${keys[0][3].join(", ")}]
        [5]  = { [false] = '${keys[0][4][1]}',  [true] = "${keys[0][4][0]}", }, -- [${keys[0][4].join(", ")}]
        [6]  = { [false] = '${keys[0][5][1]}',  [true] = "${keys[0][5][0]}", }, -- [${keys[0][5].join(", ")}]
        [7]  = { [false] = '${keys[0][6][1]}',  [true] = "${keys[0][6][0]}", }, -- [${keys[0][6].join(", ")}]
        [8]  = { [false] = '${keys[0][7][1]}',  [true] = "${keys[0][7][0]}", }, -- [${keys[0][7].join(", ")}]
        [9]  = { [false] = '${keys[0][8][1]}',  [true] = "${keys[0][8][0]}", }, -- [${keys[0][8].join(", ")}]
        [10] = { [false] = '${keys[0][9][1]}',  [true] = "${keys[0][9][0]}", }, -- [${keys[0][9].join(", ")}]
        [11] = { [false] = '${keys[0][10][1]}',  [true] = "${keys[0][10][0]}", }, -- [${keys[0][10].join(", ")}]
        [12] = { [false] = '${keys[0][11][1]}',  [true] = "${keys[0][11][0]}", }, -- [${keys[0][11].join(", ")}]
        [13] = { [false] = '${keys[0][12][1]}',  [true] = "${keys[0][12][0]}", }, -- [${keys[0][12].join(", ")}]
        [16] = { [false] = '${keys[1][1][1]}',  [true] = "${keys[1][1][0]}", }, -- [${keys[1][1].join(", ")}]
        [17] = { [false] = '${keys[1][2][1]}',  [true] = "${keys[1][2][0]}", }, -- [${keys[1][2].join(", ")}]
        [18] = { [false] = '${keys[1][3][1]}',  [true] = "${keys[1][3][0]}", }, -- [${keys[1][3].join(", ")}]
        [19] = { [false] = '${keys[1][4][1]}',  [true] = "${keys[1][4][0]}", }, -- [${keys[1][4].join(", ")}]
        [20] = { [false] = '${keys[1][5][1]}',  [true] = "${keys[1][5][0]}", }, -- [${keys[1][5].join(", ")}]
        [21] = { [false] = '${keys[1][6][1]}',  [true] = "${keys[1][6][0]}", }, -- [${keys[1][6].join(", ")}]
        [22] = { [false] = '${keys[1][7][1]}',  [true] = "${keys[1][7][0]}", }, -- [${keys[1][7].join(", ")}]
        [23] = { [false] = '${keys[1][8][1]}',  [true] = "${keys[1][8][0]}", }, -- [${keys[1][8].join(", ")}]
        [24] = { [false] = '${keys[1][9][1]}',  [true] = "${keys[1][9][0]}", }, -- [${keys[1][9].join(", ")}]
        [25] = { [false] = '${keys[1][10][1]}',  [true] = "${keys[1][10][0]}", }, -- [${keys[1][10].join(", ")}]
        [26] = { [false] = '${keys[1][11][1]}',  [true] = "${keys[1][11][0]}", }, -- [${keys[1][11].join(", ")}]
        [27] = { [false] = '${keys[1][12][1]}',  [true] = "${keys[1][12][0]}", }, -- [${keys[1][12].join(", ")}]
        [30] = { [false] = '${keys[2][1][1]}',  [true] = "${keys[2][1][0]}", }, -- [${keys[2][1].join(", ")}]
        [31] = { [false] = '${keys[2][2][1]}',  [true] = "${keys[2][2][0]}", }, -- [${keys[2][2].join(", ")}]
        [32] = { [false] = '${keys[2][3][1]}',  [true] = "${keys[2][3][0]}", }, -- [${keys[2][3].join(", ")}]
        [33] = { [false] = '${keys[2][4][1]}',  [true] = "${keys[2][4][0]}", }, -- [${keys[2][4].join(", ")}]
        [34] = { [false] = '${keys[2][5][1]}',  [true] = "${keys[2][5][0]}", }, -- [${keys[2][5].join(", ")}]
        [35] = { [false] = '${keys[2][6][1]}',  [true] = "${keys[2][6][0]}", }, -- [${keys[2][6].join(", ")}]
        [36] = { [false] = '${keys[2][7][1]}',  [true] = "${keys[2][7][0]}", }, -- [${keys[2][7].join(", ")}]
        [37] = { [false] = '${keys[2][8][1]}',  [true] = "${keys[2][8][0]}", }, -- [${keys[2][8].join(", ")}]
        [38] = { [false] = '${keys[2][9][1]}',  [true] = "${keys[2][9][0]}", }, -- [${keys[2][9].join(", ")}]
        [39] = { [false] = '${keys[2][10][1]}',  [true] = "${keys[2][10][0]}", }, -- [${keys[2][10].join(", ")}]
        [40] = { [false] = '${keys[2][11][1]}', [true] = "${keys[2][11][0]}", }, -- [${keys[2][11].join(", ")}]
        [41] = { [false] = '${keys[0][0][1]}',  [true] = "${keys[0][0][0]}", }, -- [${keys[0][0].join(", ")}]
        [43] = { [false] = '${keys[2][12][1]}', [true] = "${keys[2][12][1]}", }, -- [${keys[2][12].join(", ")}] DO NOT use '|' char. It's reserved for cursor 
        [44] = { [false] = '${keys[3][2][1]}',  [true] = "${keys[3][2][0]}", }, -- [${keys[3][2].join(", ")}]
        [45] = { [false] = '${keys[3][3][1]}',  [true] = "${keys[3][3][0]}", }, -- [${keys[3][3].join(", ")}]
        [46] = { [false] = '${keys[3][4][1]}',  [true] = "${keys[3][4][0]}", }, -- [${keys[3][4].join(", ")}]
        [47] = { [false] = '${keys[3][5][1]}',  [true] = "${keys[3][5][0]}", }, -- [${keys[3][5].join(", ")}]
        [48] = { [false] = '${keys[3][6][1]}',  [true] = "${keys[3][6][0]}", }, -- [${keys[3][6].join(", ")}]
        [49] = { [false] = '${keys[3][7][1]}',  [true] = "${keys[3][7][0]}", }, -- [${keys[3][7].join(", ")}]
        [50] = { [false] = '${keys[3][8][1]}',  [true] = "${keys[3][8][0]}", }, -- [${keys[3][8].join(", ")}]
        [51] = { [false] = '${keys[3][9][1]}',  [true] = "${keys[3][9][0]}", }, -- [${keys[3][9].join(", ")}]
        [52] = { [false] = '${keys[3][10][1]}',  [true] = "${keys[3][10][0]}", }, -- [${keys[3][10].join(", ")}]
        [53] = { [false] = '${keys[3][11][1]}',  [true] = "${keys[3][11][0]}", }, -- [${keys[3][11].join(", ")}]
        [57] = { [false] = ' ',  [true] = " ", },
    },
`

console.log(result)