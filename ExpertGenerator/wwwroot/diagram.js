var DRAW_IFRAME_URL = 'https://embed.diagrams.net/?embed=1';
var graph = null;


function edit(xml)
{
    const diagramArea = document.getElementById('diagram-area');
    console.log("sadd")
    var border = 0;
    var iframe = document.createElement('iframe');
    iframe.style.zIndex = '9999';
    iframe.style.position = 'relative';
    iframe.style.top = border + 'px';
    iframe.style.left = border + 'px';

    if (border == 0)
    {
        iframe.setAttribute('frameborder', '0');
    }

    var resize = function()
    {
        iframe.setAttribute('width', "100%");
        iframe.setAttribute('height', "1000px");
    };

    window.addEventListener('resize', resize);
    resize();

    var receive = function(evt)
    {
        console.log(evt)
        if (evt.data == 'ready')
        {
            console.log(xml)
            iframe.contentWindow.postMessage(xml, '*');
            resize();
        }
        else
        {
            if (evt.data.length > 0)
            {
                var xmlDoc = mxUtils.parseXml(evt.data);
                var codec = new mxCodec(xmlDoc);
                codec.decode(codec.document.documentElement, graph.getModel());
                graph.fit();
                graph.center(true, false);

            }

            window.removeEventListener('resize', resize);
            window.removeEventListener('message', receive);
            diagramArea.removeChild(iframe);
        }
    };

    window.addEventListener('message', receive);
    iframe.setAttribute('src', DRAW_IFRAME_URL);
    diagramArea.appendChild(iframe);
}

function loadDiagram() {
    
    fetch('/download/diagram')
        .then(response => {
            console.log("Hi")
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(xml => {    
            console.log(xml)
            edit(xml);
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}
console.log("STARTTTT")
var doc = document.documentElement.outerHTML;