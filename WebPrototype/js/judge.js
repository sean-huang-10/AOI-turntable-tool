// Same token-based comparison as Assets/Scripts/Judge/RuleBasedJudge.cs, so
// "bool machineReady=true;" and "bool machineReady = true;" both count as
// correct, while "machine Ready" (a different token) does not.
function tokenize(code) {
  return code.match(/\w+|[^\s\w]/g) || [];
}

function isCorrect(playerAnswer, expectedAnswer) {
  if (playerAnswer == null || expectedAnswer == null) {
    return false;
  }

  const playerTokens = tokenize(playerAnswer);
  const expectedTokens = tokenize(expectedAnswer);

  if (playerTokens.length !== expectedTokens.length) {
    return false;
  }

  return playerTokens.every((token, i) => token === expectedTokens[i]);
}
