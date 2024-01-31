import java.util.List;

public class GenerateV implements Runnable{
    private List<Integer> a;
    private boolean onev;
    
    public GenerateV(List<Integer> a, boolean onev){
        this.a = a;
        this.onev = onev;
    }

    @Override
    public void run() {
        if(onev) {
            for (int i = 0; i < 30; i++) {
                a.add(1);
            }
        }
        else {
            for (int i = 0; i < 30; i++) {
                a.add(i + 1);
            }
        }
    }

    public List<Integer> getA() {
        return a;
    }
}
