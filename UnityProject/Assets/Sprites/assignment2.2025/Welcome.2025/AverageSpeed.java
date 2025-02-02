public class AverageSpeed {
    public static void main(String[] args) { 
        int d1 = Integer.parseInt(args[0]);
        int d2 = Integer.parseInt(args[1]);
        int t = Integer.parseInt(args[2]);
        
        if(t <= 0)
        {
            System.out.println("error");
            return; 
        }
        else
        {
            double v = (double) (d2-d1)/t;
            System.out.println(v);
        }
    }
}