// Mirrors CSharpEngineerQuest/Assets/Resources/Levels/*.json - keep both in
// sync when adding or editing levels. Plain JS objects (not fetched JSON) so
// this file works when opened directly from disk, with no local server.
const LEVELS = [
  {
    levelId: 1,
    worldId: 1,
    title: "設備啟動",
    topic: "bool",
    difficulty: 1,
    questionType: "FillCode",
    story: "你剛接手一台無法啟動的 AOI 設備。工程師留下的程式缺少設備 Ready 狀態。請建立 machineReady 變數。",
    question: "建立 bool machineReady 並設定為 true。",
    starterCode: "",
    expectedAnswer: "bool machineReady = true;",
    hints: [
      "這個狀態只有 true / false。",
      "可以使用 bool。",
      "bool machineReady = ___;"
    ],
    xpReward: 100
  },
  {
    levelId: 2,
    worldId: 1,
    title: "啟動馬達",
    topic: "if",
    difficulty: 1,
    questionType: "FillCode",
    story: "設備已成功啟動。現在必須確認設備 Ready 後才可以啟動馬達。",
    question: "請完成判斷式，確認 machineReady 為 true 時才呼叫 StartMotor()。",
    starterCode: "bool machineReady = true;\n___________\n{\n    StartMotor();\n}",
    expectedAnswer: "bool machineReady = true;\nif (machineReady)\n{\n    StartMotor();\n}",
    hints: [
      "只有在條件成立時，裡面的程式才會執行。",
      "可以使用 if 陳述式。",
      "if (___)"
    ],
    xpReward: 120
  }
];
