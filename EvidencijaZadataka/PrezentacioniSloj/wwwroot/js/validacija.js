const REGEX = {
    email:         /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/,
    korisnickoIme: /^[a-zA-Z0-9_]{3,50}$/,
    lozinka:       /^(?=.*[a-zA-Z])(?=.*\d).{6,}$/,
    naziv:         /\S/
};

function prikaziGresku(id, poruka) {
    const el = document.getElementById(id);
    if (el) el.textContent = poruka;
}

function obrisiGresku(id) {
    const el = document.getElementById(id);
    if (el) el.textContent = "";
}

function obrisiSveGreske() {
    document.querySelectorAll("[id^='greska-']")
        .forEach(el => el.textContent = "");
}


function validirajFormu() {
    obrisiSveGreske();
    let ispravno = true;

    const naziv = document.getElementById("naziv")?.value?.trim() ?? "";
    if (naziv.length < 5) {
        prikaziGresku("greska-naziv",
            "Naziv mora imati najmanje 5 karaktera.");
        ispravno = false;
    } else if (naziv.length > 150) {
        prikaziGresku("greska-naziv",
            "Naziv ne sme biti duži od 150 karaktera.");
        ispravno = false;
    }

    const rokVr = document.getElementById("rokZaZavrsetak")?.value ?? "";
    if (!rokVr) {
        prikaziGresku("greska-rok", "Rok za završetak je obavezan.");
        ispravno = false;
    } else {
        const danas = new Date();
        danas.setHours(0, 0, 0, 0);
        if (new Date(rokVr) < danas) {
            prikaziGresku("greska-rok",
                "Rok za završetak ne može biti u prošlosti.");
            ispravno = false;
        }
    }

    const procenat = parseInt(
        document.getElementById("procenat")?.value ?? "0", 10);
    if (isNaN(procenat) || procenat < 0 || procenat > 100) {
        prikaziGresku("greska-procenat",
            "Procenat Zavrsenosti mora biti između 0 i 100.");
        ispravno = false;
    }

    let losihStavki = 0;
    document.querySelectorAll(".naziv-stavke").forEach(polje => {
        const v = polje.value.trim();
        if (v !== "" && v.length < 3) {
            polje.style.borderColor = "red";
            losihStavki++;
        } else {
            polje.style.borderColor = "";
        }
    });

    if (losihStavki > 0) {
        prikaziGresku("greska-naziv",
            "Svaki podzadatak mora imati naziv sa najmanje 3 karaktera.");
        ispravno = false;
    }

    return ispravno;
}

function validirajRegistraciju() {
    obrisiSveGreske();
    let ispravno = true;

    const korisnickoIme =
        document.getElementById("korisnickoIme")?.value?.trim() ?? "";
    if (!REGEX.korisnickoIme.test(korisnickoIme)) {
        prikaziGresku("greska-korisnicko-ime",
            "Korisničko ime: 3–50 znakova, samo slova, cifre i _ .");
        ispravno = false;
    }

    const email = document.getElementById("email")?.value?.trim() ?? "";
    if (!REGEX.email.test(email)) {
        prikaziGresku("greska-email",
            "Unesite ispravnu email adresu (primer: ime@domen.rs).");
        ispravno = false;
    }

    const lozinka = document.getElementById("lozinka")?.value ?? "";
    const potvrdaLozinke =
        document.getElementById("potvrdaLozinke")?.value ?? "";

    if (!REGEX.lozinka.test(lozinka)) {
        prikaziGresku("greska-lozinka",
            "Lozinka mora imati najmanje 6 karaktera, " +
            "bar jedno slovo i jednu cifru.");
        ispravno = false;
    } else if (lozinka !== potvrdaLozinke) {
        prikaziGresku("greska-lozinka", "Lozinke se ne poklapaju.");
        ispravno = false;
    }

    return ispravno;
}


document.addEventListener("DOMContentLoaded", function () {

    const emailPolje = document.getElementById("email");
    if (emailPolje) {
        emailPolje.addEventListener("blur", function () {
            if (this.value && !REGEX.email.test(this.value.trim()))
                prikaziGresku("greska-email", "Unesite ispravnu email adresu.");
            else
                obrisiGresku("greska-email");
        });
    }

    const imenPolje = document.getElementById("korisnickoIme");
    if (imenPolje) {
        imenPolje.addEventListener("blur", function () {
            if (this.value && !REGEX.korisnickoIme.test(this.value.trim()))
                prikaziGresku("greska-korisnicko-ime",
                    "Korisničko ime: 3–50 znakova, slova, cifre i _ .");
            else
                obrisiGresku("greska-korisnicko-ime");
        });
    }

    const rokPolje = document.getElementById("rokZaZavrsetak");
    if (rokPolje) {
        rokPolje.addEventListener("change", function () {
            const danas = new Date();
            danas.setHours(0, 0, 0, 0);
            if (this.value && new Date(this.value) < danas)
                prikaziGresku("greska-rok",
                    "Rok ne može biti u prošlosti.");
            else
                obrisiGresku("greska-rok");
        });
    }

    const procenatPolje = document.getElementById("procenat");
    if (procenatPolje) {
        procenatPolje.addEventListener("input", function () {
            const v = parseInt(this.value, 10);
            if (isNaN(v) || v < 0 || v > 100)
                prikaziGresku("greska-procenat",
                    "Procenat mora biti između 0 i 100.");
            else
                obrisiGresku("greska-procenat");
        });
    }
});