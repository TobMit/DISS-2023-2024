// See https://aka.ms/new-console-template for more information

// mať jeden generrátor ktorý bude generovať násady
// nása da sa používa keď ladím, keď neladím tak sa nepoužívame násadu

// treba mať generátor pre každé čislo, čiže nezahadzujem čísla
//Pri generovaní čísel je potrebné dodržať minimálne tieto požiadavky:
// 1. Pre každý náhodný jav máme jeden generátor náhodných čísel, tento sa v niektorých prípadoch, resp. pri použití niektorých rozdelení môže skladať z ďalších generátorov.
// 2. Použitie už vytvoreného generátora nikdy nemeníme.
//     Napr. nie je dovolené volať nad jednou inštanciou rôzne parametre. Random1.NexInt(11) a následne Random1.NexInt(10).
// 3. Dôsledne dodržiavame zaradenie čísel na krajoch intervalu.
// 4. Každý generátor je nutné práve raz nainicializovať "kvalitnou" násadou. Počas vykonávanie replikácií nie je dovolené opätovné vytváranie generátorov, alebo reinicializácia.
// 5. Náhodné čísla "nezahadzujeme". Vygenerujeme vždy číslo z takého rozsahu aký je vhodný.


// Simulačné jadro (zabezpečí beh simulácie):
// v tomoto prípade je to for cyklus, tak aby to bolo všeobecne použiteľné

// Symulačé jadro
    // nie je abstraktná, je reálne naimplementovaná
    // vo vnútri je simuluj
    // vykoná nejakú replikáciu
    // potrebujeme metódy, replikácia, predReplikácia, afterReplikácia, predVšetkyýmiReplikáciam, poVšetkýchReplikáciach
    // môžeme to robiť cez interface alebo cez dedičnosť, všetko prerkývame ale metódu simuluj neprekrývame

// potebujeme grafy, stačí čiarový graf aby to bolo prehľadné lifecharts
    // škálovanie funguje automaticky, škála sa prehodnocuje tak ako sa mení graf
    // na začiatk sa ustálovanie zobrazuje, odrezanie prvých x replikácií
    // na graf nevykreslujem všetky dáta, treba určiť si rozumnú vzorkovaciu stratégiu
        // zobrazuje sa každá 10 replikácia 

// každá stratégia nech má vlastný graf
// mať tlačidko play a stop, nemusí sa to dať opetovane spustiť
// Užívateľ
    // počet replikácií
    // orezanie
    // aby to vypysovalo číslenú hodnotu (ideálne na intervale spolahlivosti ale nie je to zadané)

// Aké generátory treba použiť
    // spojitý spojitý rovnomerny
    // spojity emyprický
    // ...
    // !!!!!! overiť si to v imput analyzesri
    // keď máme pravdepodobnosť že sa mi delí <0,1) a chcem pravdepodobnosť 60% tak používam podmienku x < 0,6
    // ak by sme tam dali <= 0.6 tak by nám to mohlo robiť problémyí


// treba to poslať do 10



//--------------------------------------------------------------------------------------------------------------
// Udalostná simulácia
// udalosti, príchod zákazníka, začiatok OBS, koniec OBS
    // prioritý front so C#
// komponenta front - ľudia sa pridávali
// príde zákazník, je možné obslúžiť, tak je obslúžená
// ak je obsadené tak ide do frontu
    // prichod zákzaníka pride v čase 0 a obsluha je voľná tak aj začiatok obsluhy je v čase 0, udalosť nemá žiadne časové trvanie
    // naplánuje sa trvanie obsluhy
    // naplánuje sa vytiahnutie daľšieho človeka z frontu, ak tam nikto nie je tak je to príchod nového človeka
// pokračovať na jadre v sem 1, metóda symuluj bude ťahať udalosti s frontu, a udaloť má mať metódu execude
// udalosť má prístup k jadru a vie plánovať udalosť
// jadro má mať 2 možnosti
// buď skončí keď dôjde čas alebo ak je nejaká interakcia s gui
// je potrebneé to zastaviť, znova spustiť, alebo vypnúť bežanie času - budúci T

// generátory
// exponenciálne
    // expomenciálne má lambdu a paramterer je 1/lambda
    // hodnota hovorí ako často chodia zákazníci (100 je zhruba medzi jedným a druhým)
    // keď máme paramter že prislo 12 z h tak to musíme preopčítať na min medzi nimi

// potrebujeme si pamätať nejaké parametre
// všetko čo sa odohráva v SJ tak sa má vyťiahnúť na gui
//štatistiky - mali by sme to mať z mas, nedokažeme exp s realým systémom
    // zmeníme vstupný parameter, vieme počítať int spoľahlivosti
    // 2 typi šatatistiky
        // vážený priemer
        // priemer klasický - súčet / počet
// budeme merať hlavne
    // dĺžka frontu = vážený priemer - musíme si pamätať kedy som naposeldy niečo pridal, budeme tam mať parameter s časom, hore bude čas a počet, a dole je celkový čas, bolo by to fajn mať triedu ktorá to bude riešiť
    // celkový čas v systéme = klasický priemer
    // čas čakania vo fronte = klasický priemer
// keď je čas tak do štatistiky dávam čas a keď tam nie je čas tak tam čas dávam
// môže sa stať že do simulačného behu sa nepridá nič, treba si kontrolovať že za celý čas sa nič nepridlo tak to tam nedám
// budeme mať štatistiku za jeden sim beh tak potom to tam dám za celý čas

// Koniec Obs
    // -> sim core
    // -> execcute
    // -> sumcore add uudalosť
// príchod zákauzníka generuje daľší príchod zákazníka

// stavy jednotlivých osôb
    // štandardne, každá osoba má tabuľku, (tabuľka so stavami, a v tých riadkoch sa menia stav, je to, prišiel, odyšiel
    // na záver ho buď vyhodíme alebo alebo mu dávama stav že odyšiel
    // na kontrolu stačí iba aktuálny stav, netreba dĺžky radu

// dĺžka radu sa menní iba v keď z nej príde, alebo odýde, je to vážená dĺžka

// graf potrebujeme iba na hodnota stráveného v pokladni

// moznosti zahrievania
    // v čase 0 si vygenerujeme kedy príde nový zákazník
// chladenie
    // pozeráme kedy sa systém vypne

// iterval spolahlivosti - povedať s akou pravdepodobnosťou sa nachádzame v tomto výsledku
    // treba ho zostrojiť, výberová smerodajná odchylka (ako sa líšime od strednej hodnoty) nie je nič okrem priemeru v Itej replikácií
    // (druhý vzorec) potrebujem súčet všetkých 2 mocnín, namiesto poľa stačí si pametať 2 hodnoty
    // potrebujeme priemer, s - odyľka, t - tabuľková hodnota studentovho rozdelenia
    // ak je všetko správne tak priemer by mal byť presne v strede
    // alfa by mala byť 1,96 podľa takej kalkulačky
    // treba si to zobrať ako potomka dajakej štatistiky
    // treba to mať implementované že ak chcem mať 99% intreval tak sa to potom prepočíta

// spomalovanie a zrýchlovanie
    // systemový event - vždy keď sa vytvorí tak sa všetko zamrzne na nejaký čas (par milisekúnt) keď budeme spomalovať tak sleep bude
    // čas zaspatia bude stabiliný -> vo vnútri neho si to uspím, on bude plánovať sám seba stále, budeme nastavovať (slider tak podľa toho to prepočítam)
    // parameter bude hovoriť ako často naplánujem daľší systémovy event
    // minimálne asi ej tak ž 1s v simulácií je 1 s v realite

// sú tam 2 režimi zobrazovania a je nutné vedieť medzi nimi prehadzovať aj za behu

// pre každú pokladňu mám vlastný generátor
// generátori sú iba raz nainicializované
// pred každou ičou si iba zresetuješ model
// event action delegat treba actions<>

// zadanie, na otestovanie
// obsluha 4 min

Console.WriteLine("Hello, World!");