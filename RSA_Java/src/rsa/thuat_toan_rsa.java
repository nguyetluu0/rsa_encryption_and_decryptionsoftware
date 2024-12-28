package rsa;

import java.io.UnsupportedEncodingException;
import java.math.BigInteger;
import java.security.SecureRandom;
import java.util.Arrays;

import jdk.internal.org.jline.terminal.TerminalBuilder;


public class thuat_toan_rsa {

    private BigInteger n, d, e, p, q, m;
   
    public BigInteger getP() {
        return p;
    }

    public void setP(BigInteger p) {
        this.p = p;
    }
    public BigInteger getQ() {
        return q;
    }

    public void setQ(BigInteger q) {
        this.q = q;
    }
    public BigInteger getE() {
        return e;
    }

    public void setE(BigInteger e) {
        this.e = e;
    }
    public BigInteger getN() {
        return n;
    }

    public void setN(BigInteger n) {
        this.n = n;
    }
    
    public BigInteger getM() {
        return m;
    }

    public void setM(BigInteger m) {
        this.m = m;
    }

    public BigInteger getD() {
        return d;
    }

    public void setD(BigInteger d) {
        this.d = d;
    }

    

    public thuat_toan_rsa(BigInteger newp,BigInteger newq, BigInteger newn, BigInteger newe, BigInteger newm, BigInteger newd) {
        p=newp;
        q=newq;
        n = newn;
        e = newe;
        m = newm;
        d = newd;
    }
    
    public thuat_toan_rsa() {     
       
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
    
    //Euclid mở rộng
    private static BigInteger[] extendedEuclid(BigInteger a, BigInteger b) {
        if (b.equals(BigInteger.ZERO)) {
            return new BigInteger[]{a, BigInteger.ONE, BigInteger.ZERO};
        }

        BigInteger[] result = extendedEuclid(b, a.mod(b));
        BigInteger gcd = result[0];
        BigInteger x = result[2];
        BigInteger y = result[1].subtract(a.divide(b).multiply(result[2]));

        return new BigInteger[]{gcd, x, y};
    }
    
    //Tìm nghịch đảo modulo
    public static BigInteger modInverse(BigInteger a, BigInteger m) {
        
        // Sử dụng thuật toán Euclid mở rộng để tìm nghịch đảo và ước chung
        BigInteger[] result = extendedEuclid(a, m);
        
        // Kiểm tra xem a và m có ước chung không
        BigInteger gcd = result[0];
        if (!gcd.equals(BigInteger.ONE)) {
            throw new IllegalArgumentException("a and m are not relatively prime");
        }

        BigInteger x = result[1];

        // Nghịch đảo modulo của a (mod m) là x
        return x.mod(m).add(m).mod(m);  // Đảm bảo x luôn dương
}
    
    private static BigInteger modPow(BigInteger base, BigInteger exponent, BigInteger modulus) {
        if (modulus.compareTo(BigInteger.ZERO) <= 0) {
            throw new IllegalArgumentException("modulus must be positive");
        }

        BigInteger result = BigInteger.ONE;
        base = base.mod(modulus);

        while (exponent.compareTo(BigInteger.ZERO) > 0) {
            if (exponent.testBit(0)) {
                result = result.multiply(base).mod(modulus);
            }
            exponent = exponent.shiftRight(1);
            base = base.multiply(base).mod(modulus);
        }
        return result;
    }
    
    
    
//    public synchronized String encrypt(String message) {
//        return modPow((new BigInteger(message.getBytes())),e,n).toString();
//    }
//
//
//    public synchronized BigInteger encrypt(BigInteger message) {
//        return modPow(message,e,n);
//    }
//    
//    //Giải mã tin nhắn văn bản, sử dụng giải mã khóa riêng
//   
//    public synchronized String decrypt(String message) {
//        return new String(modPow((new BigInteger(message)),d, n).toByteArray());
//    }
//
//    public synchronized BigInteger decrypt(BigInteger message) {
//        return modPow(message,d, n);
//    }
        //Mã hóa tin nhắn văn bản gốc, sử dụng giải mã khóa chung
        public synchronized String encrypt(String message) {
            BigInteger messageBigInt = new BigInteger(message.getBytes());
            return modPow(messageBigInt, e, n).toString();
        }

        public synchronized BigInteger encrypt(BigInteger message) {
            return modPow(message, e, n);
        }

        public synchronized String decrypt(String message) {
        BigInteger encryptedBigInt = new BigInteger(message);
        return new String(modPow(encryptedBigInt, d, n).toByteArray());
    }

        public synchronized BigInteger decrypt(BigInteger message) {
        return modPow(message, d, n);
    }

    
    
    
    
    //Tạo khóa
    public void KeyRSA(int bits){
        
        
        SecureRandom r = new SecureRandom();//Tạo số r random
        p = new BigInteger(bits , 100, r);
        q = new BigInteger(bits , 100, r);
        
        n = p.multiply(q);
        m = (p.subtract(BigInteger.ONE)).multiply(q.subtract(BigInteger.ONE)); //m là phi n
        
        boolean found = false;
        do {
            e = new BigInteger(bits , 50, r);
            if (m.gcd(e).equals(BigInteger.ONE) && e.compareTo(m) < 0) {
                found = true;
            }
        } while (!found);
        d = modInverse(e,m);
    }
    

    
 

     // Test program.
  
    public static void main(String[] args) throws Exception {
        thuat_toan_rsa rsa = new thuat_toan_rsa();
        rsa.KeyRSA(1024);
        String text ="Xin chào";
        
        
        
        System.out.println(rsa.p);
        System.out.println(rsa.q);
        System.out.println(rsa.e);
        String mahoa=rsa.encrypt(text);
        System.out.println("Đây là kết quả đoạn mã hóa:"+mahoa);
        System.out.println("Giải mã được như sau: "+rsa.decrypt(mahoa));
       
    }

    void setN(int bitleg) {
        throw new UnsupportedOperationException("Not supported yet.");
    }
    
    
}
