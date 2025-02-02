public class AmusementPark {
    public static void main(String[] args) {

        int familySize = Integer.parseInt(args[0]);
        int numUnderFive = Integer.parseInt(args[1]);
        int numSeniors = Integer.parseInt(args[2]);

        if ((numUnderFive + numSeniors) > familySize) 
        {
            System.out.println("error");
            return;
        } 
        else 
        {
            boolean isWeekend = Boolean.valueOf(args[3]);
            double total = 0; 
            if(isWeekend)
            {
                total += 25; 
            }

            total = total + (numSeniors*20);
            int stdFeePeople = familySize - numUnderFive - numSeniors;
            total = total + stdFeePeople * 35;

            if(familySize >= 5)
            {
                total = 0.9 * total;
            }
            System.out.println(total);
        }   
    }
}