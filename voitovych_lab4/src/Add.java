import java.util.List;

public class Add implements Runnable{
    private final List<Integer> a;
    private final List<Integer> b;
    private final List<Integer> c;
    public Add(List<Integer> a, List<Integer> b, List<Integer> c) {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    @Override
    public void run() {
        for (int i = 0; i < 30; i++) {
            c.add(a.get(i) + b.get(i));
        }
    }
}
