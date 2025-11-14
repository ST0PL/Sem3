
import enums.Caliber;
import java.io.IOException;
import static java.lang.System.out;
import unit.Unit;
import warehouse.resources.Ammunition;

class Lab6 {
    public static void main(String[] args) throws IOException, CloneNotSupportedException {
        var ammo = new Ammunition(1, "патроны 5.45мм", Caliber.C_545MM,100);

        var ammoClone = ammo.clone();
        out.println(ammo.getId());
        out.println(ammoClone.getId());
    }

    public static void printTree(Unit parent, String tabs){
        
        out.println(tabs+parent.getName());

        
        for(var soldier : parent.getPersonnel()){
            out.println(tabs+'\t'+soldier.toString());
        }

        if(!parent.getChildren().isEmpty()){
            for(var child : parent.getChildren()){
                printTree(child, tabs+'\t');
            }
        }        
    }
}