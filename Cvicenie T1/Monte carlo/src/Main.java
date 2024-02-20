import java.util.Random;

public class Main {
    public static final int D = 1000;
    public static final int DlzkaIhly = 10;
    public static Random random;
    public static int pocetPokusov = 1000000;

    public static void main(String[] args) {
        random = new Random();
        System.out.println(getPi());
    }
    private static double getPi() {
        int pocetPretatych = 0;
        for (int i = 0; i < pocetPokusov; i++) {
            double y = random.nextDouble() * D;
            double alfa = random.nextDouble() * 180;
            double a = Math.sin(Math.toRadians(alfa)) * DlzkaIhly;
            if (a + y >= D) {
                pocetPretatych++;
            }
        }
        return (2 * DlzkaIhly) / (D * (pocetPretatych / (double) pocetPokusov));
    }
}