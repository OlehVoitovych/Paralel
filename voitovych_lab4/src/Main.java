import java.util.ArrayList;
import java.util.List;
//Q = 7X + 2Y

public class Main {
    private static List<Integer> X,Y,Q;

    public static void main(String[] args) throws Exception {
        X = new ArrayList<>();
        Y = new ArrayList<>();
        Q = new ArrayList<>();

        GenerateV gv1 = new GenerateV(X, true);
        GenerateV gv2 = new GenerateV(Y,false);

        Thread t1 = new Thread(gv1);
        Thread t2 = new Thread(gv2);

        t1.start();
        t2.start();
        t1.join();
        t2.join();

        if(!t2.isAlive() && !t1.isAlive()){
            System.out.print("X = ");
            for (Integer i:X) {
               System.out.print(i + " ");
            }
            System.out.println();
            System.out.print("Y = ");
            for (Integer i:Y) {
                System.out.print(i + " ");
            }
            System.out.println();
        }

        Mult mx = new Mult(X, 7);
        Mult my = new Mult(Y, 2);
        t1 = new Thread(mx);
        t2 = new Thread(my);
        t1.start();
        t2.start();
        t1.join();
        t2.join();

        if(!t2.isAlive() && !t1.isAlive()){
            System.out.print("7X = ");
            for (Integer i:X) {
                System.out.print(i + " ");
            }
            System.out.println();
            System.out.print("2Y = ");
            for (Integer i:Y) {
                System.out.print(i + " ");
            }
            System.out.println();
        }

        Add q = new Add(X,Y,Q);
        t1 = new Thread(q);
        t1.start();
        t1.join();

        if(!t1.isAlive()){
            System.out.print("Q: (7X + 2Y) = ");
            for (Integer i:Q) {
                System.out.print(i + " ");
            }
        }

    }
}

