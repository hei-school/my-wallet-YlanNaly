const fs = require('fs');
const readline = require('readline');

class WalletModel {
    constructor() {
        this.wallet = {};
        this.deps = {};
        this.source = {};
    }

    addSource(name, nameSource, value) {
        const key = `${name}-${nameSource}`;
        if (this.wallet.hasOwnProperty(name)) {
            const actualValue = this.wallet[name];
            this.wallet[name] = actualValue + value;
        } else {
            this.wallet[name] = value;
        }
        this.source[key] = value;
        console.log(`Ajouté ${value} de ${name} à votre portefeuille.`);
    }

    calculateTotal(lst) {
        return lst.reduce((acc, value) => acc + value, 0);
    }

    addExpense(name, nameExpense, value) {
        const key = `${name}-${nameExpense}`;
        if (this.wallet.hasOwnProperty(name)) {
            const actualValue = this.wallet[name];
            if (0.0 < value && value < actualValue) {
                this.wallet[name] = actualValue - value;
                this.deps[key] = value;
                console.log(`Ajouté ${value} de dépense ${name} à votre portefeuille.`);
            } else if (value < 0.0) {
                console.log("Dépense 0 ariary ?");
            } else if (actualValue < value) {
                console.log(`Trop peu pour acheter quoi que cela soit :\nLe solde : ${actualValue}\nTon soit disant dépense : ${value}`);
            }
        }
    }

    getAmountsForWallet(name) {
        const listAmounts = Object.entries(this.wallet)
            .filter(([key, value]) => key.startsWith(name))
            .map(([key, value]) => value.getAmountsForWallet(name));
    
        const totalAmount = listAmounts.reduce((acc, value) => acc + value, 0);
        return totalAmount;
    }
    

    getListExpense(name) {
        const listExpense  = Object.entries(this.wallet)
            .filter(([key, value]) => key.startsWith(name))
            .map(([key, value]) => value.getListExpense(name));

        Object.entries(this.deps).forEach(([key, value]) => {
            if (key.startsWith(name)) {
                console.log("------------- ID -------------");
                console.log(key);
                console.log("---------- DEPENSES ----------");
                console.log(value);
            }
        });
        console.log("---------- TOTAL -------------");
        console.log(listExpense.length);
        console.log("------------------------------");
    }

    getListSource(name) {
        const listSource = Object.values(this.source).filter(key => key.startsWith(name));
        Object.entries(this.source).forEach(([key, value]) => {
            if (key.startsWith(name)) {
                console.log("------------- ID -------------");
                console.log(key);
                console.log("---------- REVENUES ----------");
                console.log(value);
            }
        });
        console.log("---------- TOTAL -------------");
        console.log(listSource.length);
        console.log("------------------------------");
    }

    setAmount(value) {
        this.wallet[value] = 0.0;
    }
}

class Action {
    constructor() {
        this.wallet = {};
        this.FILENAME = "wallet.json";
        this.chargeData();
    }

    chargeData() {
        try {
            const fs = require('fs');
            const data = JSON.parse(fs.readFileSync(this.FILENAME, 'utf8'));
    
            for (const [key, value] of Object.entries(data)) {
                this.wallet[key] = new WalletModel();
            }
        } catch (error) {
            console.log("Impossible de charger les données existantes.");
        }
    }

    saveData() {
        try {
            fs.writeFileSync(this.FILENAME, JSON.stringify(this.wallet));
        } catch (error) {
            console.log("Erreur lors de la sauvegarde des données.");
        }
    }

    createWallet(name) {
        this.wallet[name] = new WalletModel();
        this.saveData();
        console.log(`Portefeuille créé pour ${name}`);
    }

    addSource(name, source, value) {
        const walletModel = this.wallet[name];
        if (walletModel) {
            walletModel.addSource(name, source, value);
            this.saveData();
        } else {
            console.log(`Portefeuille introuvable pour ${name}`);
        }
    }

    expenses(name, nameExpense, value) {
        const walletModel = this.wallet[name];
        if (walletModel) {
            walletModel.addExpense(name, nameExpense, value);
            this.saveData();
        } else {
            console.log(`Portefeuille introuvable pour ${name}`);
        }
    }

    showWalletModel(name) {
        const walletModel = this.wallet[name];
        if (walletModel) {
            console.log(`Votre compte est de : ${walletModel.getAmountsForWallet(name)}`);
        } else {
            console.log(`Portefeuille introuvable pour ${name}`);
        }
    }

    showListExpenses(name) {
        const walletModel = this.wallet[name];
        if (walletModel) {
            walletModel.getListExpense(name);
        } else {
            console.log(`Portefeuille introuvable pour ${name}`);
        }
    }

    showListSource(name) {
        const walletModel = this.wallet[name];
        if (walletModel) {
            walletModel.getListSource(name);
        } else {
            console.log(`Portefeuille introuvable pour ${name}`);
        }
    }

    resetWallet(name) {
        const walletModel = this.wallet[name];
        if (walletModel) {
            walletModel.setAmount(name);
            this.saveData();
            console.log(`Portefeuille réinitialisé pour ${name}`);
        } else {
            console.log(`Portefeuille introuvable pour ${name}`);
        }
    }
}

const gestionPortefeuilles = new Action();
let action = true;

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

function menu() {
    console.log("\nMenu :");
    console.log("1. Créer un portefeuille pour une personne");
    console.log("2. Ajouter un source à un portefeuille");
    console.log("3. Ajouter une dépense à un portefeuille");
    console.log("4. Afficher le portefeuille d'une personne");
    console.log("5. Afficher la liste des dépenses d'une personne");
    console.log("6. Afficher la liste des revenus d'une personne");
    console.log("7. Vider votre portefeuille");
    console.log("8. Ranger votre portefeuille");

    rl.question("Choix : ", (choice) => {
        processInput(choice);
    });
}

function processInput(choice) {
    switch (choice) {
        case '1':
            rl.question("Nom de la personne : ", (namePerson) => {
                gestionPortefeuilles.createWallet(namePerson);
                menu();
            });
            break;
        case '2':
            rl.question("Nom de la personne : ", (nameActifPerson) => {
                rl.question("Nom du source de revenu : ", (nameSource) => {
                    rl.question("Montant : ", (montant) => {
                        gestionPortefeuilles.addSource(nameActifPerson, nameSource, parseFloat(montant));
                        menu();
                    });
                });
            });
            break;
        case '3':
            rl.question("Nom de la personne : ", (name) => {
                rl.question("Nom de la dépense : ", (nameDeps) => {
                    rl.question("Dépense : ", (deps) => {
                        gestionPortefeuilles.expenses(name, nameDeps, parseFloat(deps));
                        menu();
                    });
                });
            });
            break;
        case '4':
            rl.question("Nom de la personne : ", (nameAffichage) => {
                gestionPortefeuilles.showWalletModel(nameAffichage);
                menu();
            });
            break;
        case '5':
            rl.question("Nom de la personne : ", (nameListExpenses) => {
                gestionPortefeuilles.showListExpenses(nameListExpenses);
                menu();
            });
            break;
        case '6':
            rl.question("Nom de la personne : ", (nameListSource) => {
                gestionPortefeuilles.showListSource(nameListSource);
                menu();
            });
            break;
        case '7':
            rl.question("Nom de la personne : ", (reset) => {
                gestionPortefeuilles.resetWallet(reset);
                menu();
            });
            break;
        case '8':
            action = false;
            rl.close();
            break;
        default:
            console.log("Choix invalide.");
            menu();
            break;
    }
}

menu();
