
from https://www.quackit.com/css/css_color_codes.cfm

```javascript
names = new Set()
;[...document.querySelectorAll('.color-chart-wrapper tr')]
.filter(tr => tr.querySelectorAll('td').length == 3)
.map(tr => {
    let a = [...tr.querySelectorAll('td')].map(td => td.innerText);
    let [n,,c] = a
    return [n,c]
})
.filter(([n]) => {
    let already = names.has(n)
    names.add(n)
    return !already
})
.map(([n,c]) => {
    let [r,g,b] = c.split(',').map(v => (parseFloat(v)/0xff).toFixed(2))
    let p1 = `static const fixed4 ${n} = `.padEnd(46)
    return p1 + `fixed4(${r},${g},${b},1);`
}).join('\n')
```