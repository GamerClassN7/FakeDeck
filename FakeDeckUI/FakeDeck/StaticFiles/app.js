[].forEach.call(document.querySelectorAll('form'), function (form) {
    form.addEventListener('submit', function (event) {
        event.preventDefault();
        const target = event.currentTarget;

        console.log(target.method, target.action);
        console.log(form.method, form.action);
        console.log(form === target);

        var formData = new FormData(form);

        const xhttp = new XMLHttpRequest();
        xhttp.onload = function () { }
        xhttp.onreadystatechange = function () {
            if (xhttp.readyState === 4) {
                if (xhttp.status === 200) {
                    console.log('successful');
                } else {
                    console.log('failed');
                }
            }
        }

        xhttp.open(target.method, target.action, true);
        xhttp.send(urlencodeFormData(formData));
    });
});

function urlencodeFormData(fd){
    var s = '';
    function encode(s){ return encodeURIComponent(s).replace(/%20/g,'+'); }
    for(var pair of fd.entries()){
        if(typeof pair[1]=='string'){
            s += (s?'&':'') + encode(pair[0])+'='+encode(pair[1]);
        }
    }
    return s;
}