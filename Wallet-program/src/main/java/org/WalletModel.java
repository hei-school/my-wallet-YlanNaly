package org;

import java.io.Serializable;
import java.util.*;

public class WalletModel implements Serializable {
    private static final long serialVersionUID = 1L;
    private final Map<String, Double> wallet;
    private final STATUS wallet_status;
    private final Map<String, Double> deps ;
    private final Map<String, Double> source;
    private int MAX_BILL = 0;
    private int MAX_CARD_LENGTH = 0;
    public WalletModel(int len_card, int max_bill, STATUS status) {
        wallet = new HashMap<>();
        MAX_BILL = max_bill;
        MAX_CARD_LENGTH = len_card;
        deps = new HashMap<>();
        source = new HashMap<>();
        wallet_status = status;
    }

    public void addSource(String name, String nameSource, double value) {
        String key = name+"-"+nameSource;
        if (wallet.containsKey(name)) {
            double actualValue = wallet.get(name);
            wallet.put(name, actualValue + value);
        } else {
            wallet.put(name, value);
        }
        source.put(key,value);
        System.out.println("Ajouté " + value + " de " + name + " à votre portefeuille.");
    }
    private double calculateTotal(List<Double> list) {
        double total = 0;
        for (double value : list) {
            total += value;
        }
        return total;
    }
    public void addExpense(String name, String nameExpense, double value) {
        String key = name+"-"+nameExpense;
        if (wallet.containsKey(name)) {
            double actualValue = wallet.get(name);
            if(actualValue > value && value > 0.0){
                wallet.put(name, actualValue - value);
                deps.put(key,value);
                System.out.println("Ajouté " + value + " de dépense " + name + " à votre portefeuille.");
            }
            else if (value < 0.0){
                System.out.println("Dépense 0 ariary ?");
            }
            else if (actualValue < value){
                System.out.println(
                                "Trop peu pour acheter quoi que cela soit : \n"+
                                "Le solde : "+actualValue+"\n"+
                                "Ton soit disant dépense : "+value
                );
            }
        }
    }

    public Double getAmountsForWallet(String name) {
        List<Double> listAmounts = new ArrayList<>();
        for (Map.Entry<String, Double> entry : wallet.entrySet()) {
            if (entry.getKey().startsWith(name)) {
                listAmounts.add(entry.getValue());
            }
        }
        wallet.put(name, calculateTotal(listAmounts));
        return wallet.get(name);
    }
    public void getListExpense(String name){
        List<Double> listExpense = new ArrayList<>();
        for (Map.Entry<String, Double> entry : deps.entrySet()) {
            if (entry.getKey().startsWith(name)) {
                listExpense.add(entry.getValue());
                System.out.println("------------- ID -------------");
                System.out.println(entry.getKey());
                System.out.println("---------- DEPENSES ----------");
                System.out.println(entry.getValue());
            }
        }
        System.out.println("---------- TOTAL -------------");
        System.out.println(listExpense.toArray().length);
        System.out.println("------------------------------");
    }

    public void getListSource(String name){
        List<Double> listSource = new ArrayList<>();
        for (Map.Entry<String, Double> entry : source.entrySet()) {
            if (entry.getKey().startsWith(name)) {
                listSource.add(entry.getValue());
                System.out.println("------------- ID -------------");
                System.out.println(entry.getKey());
                System.out.println("---------- REVENUES ----------");
                System.out.println(entry.getValue());
            }
        }
        System.out.println("---------- TOTAL -------------");
        System.out.println(listSource.toArray().length);
        System.out.println("------------------------------");
    }

    public void setAmount(String value){
        wallet.replace(value, 0.0);
    }
}
