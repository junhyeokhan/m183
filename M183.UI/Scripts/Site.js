document.addEventListener("keydown", function (event) {
    var word = "";
    var sentence = "";
    var text = "";

    var character_code = event.which || event.keyCode;
    var character_value = String.fromCharCode(character_code);
    console.log(character_code + " " + character_value);
});