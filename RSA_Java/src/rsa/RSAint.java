package rsa;

import java.math.BigInteger;
import java.util.Random;

public class RSAint {
    private BigInteger n, d, e, m;

    public RSAint(BigInteger n, BigInteger e, BigInteger m, BigInteger d ) {
        this.n = n;
        this.d = d;
        this.e = e;
        this.m = m;
    }

    public BigInteger getN() {
        return n;
    }
    public BigInteger getphiN() {
        return m;
    }

    public BigInteger getD() {
        return d;
    }

    public BigInteger getE() {
        return e;
    }
     public RSAint() {
        
    }
    public static boolean isPrime(BigInteger n) {
        if (n.equals(BigInteger.ONE) || n.signum() < 1) {
            return false;
        }
        if (n.equals(BigInteger.valueOf(2))) {
            return true;
        }
        if (n.mod(BigInteger.valueOf(2)).equals(BigInteger.ZERO)) {
            return false;
        }
    
        BigInteger sqrtN = n.sqrt();
        for (BigInteger i = BigInteger.valueOf(3); i.compareTo(sqrtN) <= 0; i = i.add(BigInteger.valueOf(2))) {
            if (n.mod(i).equals(BigInteger.ZERO)) {
                return false;
            }
        }
        return true;
    }
    
    public BigInteger modularExponentiation(BigInteger base, BigInteger exponent, BigInteger modulus) {
        BigInteger result = BigInteger.ONE;
        while (exponent.compareTo(BigInteger.ZERO) > 0) {
            if (exponent.mod(BigInteger.TWO).equals(BigInteger.ONE)) {
                result = (result.multiply(base)).mod(modulus);
            }
            base = (base.multiply(base)).mod(modulus);
            exponent = exponent.divide(BigInteger.TWO);
        }
        return result;
    }

    //Tạo key thủ công
    public void generateKeyPair(BigInteger p, BigInteger q) {
        n = p.multiply(q);
        m = (p.subtract(BigInteger.ONE)).multiply(q.subtract(BigInteger.ONE));

        e = randomE(m);
        d = e.modInverse(m);

    }

    // Chọn e random
    public static BigInteger randomE(BigInteger m) {
        Random random = new Random();
        BigInteger e;
        do {
            e = new BigInteger(m.bitLength(), random);
        } while (e.compareTo(BigInteger.TWO) < 0 || e.compareTo(m) >= 0 || !e.gcd(m).equals(BigInteger.ONE));
        return e;
    }
    
    public String encrypt(String plaintext) {
    StringBuilder ciphertext = new StringBuilder();
        for (int i = 0; i < plaintext.length(); i++) {
            char character = plaintext.charAt(i);
            BigInteger charCode = BigInteger.valueOf((int) character);
            BigInteger encryptedCode = modularExponentiation(charCode, e, n);
            ciphertext.append(encryptedCode.toString()).append(" ");
        }
        return ciphertext.toString().trim(); 
    }

    public String decrypt(String ciphertext) {
        StringBuilder plaintext = new StringBuilder();
        String[] encryptedValues = ciphertext.split(" ");
        for (String encryptedValue : encryptedValues) {
            if (!encryptedValue.isEmpty()) {
                BigInteger encryptedCode = new BigInteger(encryptedValue);
                BigInteger decryptedCode = modularExponentiation(encryptedCode, d, n);
                char character = (char) decryptedCode.intValue();
                plaintext.append(character);
            }
        }
        return plaintext.toString();
    }
    

    public static void main(String[] args) {
        BigInteger p = new BigInteger("61");
        BigInteger q = new BigInteger("53");
        
        RSAint rsa=new RSAint();
        rsa.generateKeyPair(p, q);
        
        BigInteger n = rsa.getN();
        BigInteger d = rsa.getD();
        BigInteger e = rsa.getE();

        System.out.println("Public key (e): " + e);
        System.out.println("Private key (d): " + d);
        System.out.println("Modulus (n): " + n);  
        
        String plaintext = "Xin chào";
        System.out.println("Plaintext: " + plaintext);

        // Encrypt plaintext
        String encryptedText = rsa.encrypt(plaintext);
        System.out.println("Encrypted: " + encryptedText);

        // Decrypt ciphertext
        String decryptedText = rsa.decrypt(encryptedText);
        System.out.println("Decrypted: " + decryptedText);
    }

}