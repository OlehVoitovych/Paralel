import java.util.List;

public class Mult implements Runnable{
    private final List<Integer> a;
    private int k;
    public Mult(List<Integer> a, int k) {
        this.a = a;
        this.k = k;
    }

    @Override
    public void run() {
        for (int i = 0; i < 30; i++) {
            a.set(i, a.get(i) * k);
        }
    }
}
