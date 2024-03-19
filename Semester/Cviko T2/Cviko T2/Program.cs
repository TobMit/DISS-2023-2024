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

// zadanie, na otestovanie
// obsluha 4 min

Console.WriteLine("Hello, World!");