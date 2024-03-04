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
    // ak by sme tam dali <= 0.6 tak by nám to mohlo robiť problémy


// treba to poslať do 10




Console.WriteLine("Hello, World!");