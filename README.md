<h1>Opis</h1>
Moim pomysłem było zrobić podwaliny movementu do gry która mogłaby być bullethellem, roguelikiem albo coś w tym stylu.
Głównym aspektem jest budowanie swojej prędkości, by sprawniej unikać i pokonywać przeciwników.
<br/>
<br/>
Sam system walki nie jest rozbudowany, bo nie starczyło mi czasu żeby zaprojektować i zaprogramować ciekawych przeciwników.
Dziesięć dni to mało czasu na napisanie pełnego movement systemu, więc byłem zmuszony do dużej ilości uproszczeń, a przede wszystkim nie zdążyłem zbalansować zmiennych w skryptach. 
Wiele linijek kodu napisałbym zupełnie inaczej gdybym miał na to więcej czasu a kod byłby dużo bardziej uporządkowany (np. Skrypty PlayerMovementManager i Enemy dziedziczyłyby od klasy Character, która przechowywałaby ich wspólne właściwości).
<br/>
<br/>
Wydaje mi się, że wszystko w skryptach mówi samo za siebie, więc pozwoliłem sobie nie pisać dokumentacji.

<h1>Features</h1>
<h3>- Running</h3>
<h5>Opis</h2>
Co tu dużo tłumaczyć, zwykłe bieganie.
<h5>Controls</h2>
W, A, S, D
<h3>- Dashing</h3>
<h5>Opis</h2>
Dash zachowuje prędkość gracza i dodaje do niego swoją.<br/>
Pozwala na rozpędzanie się i zadaje obrażenia przeciwnikom.<br/>
Gracz ma określoną ilość dashy, które regenerują się w czasie, lub przy zabójstwie Dashem.<br/>
Gdy gracz ma zbyt dużą prędkość i wleci w ścianę, zostaje ogłuszony na kilka sekund.
<h5>Controls</h2>
PPM
<h3>- Sliding</h3>
<h5>Opis</h2>
Dasha można użyć kiedy gracz ma już określoną prędkość.<br/>
Sprawia że gracz zwalnia w czasie, lub od razu się zatrzymuje.<br/>
Podczas slide'a można używać Dasha.
<h5>Controls</h2>
Space (Hold) - Spowalnianie w czasie<br/>
Space (Release) - Gwałtowne zatrzymanie
<h3>- Shooting</h3>
<h5>Opis</h2>
Strzelanie pociskami w kierunku kursora.
<h5>Controls</h2>
LPM
