package request;

import enums.SupplyResponseStatus;
import java.util.ArrayList;
import java.util.stream.Collectors;

public class SupplyResponse {
    SupplyResponseStatus status;
    ArrayList<SupplyRequestDetail> unsatisfiedDetails;
    String comment;
    
    public SupplyResponse(SupplyResponseStatus status, String comment) {
        this.status = status;
        this.comment = comment;
        this.unsatisfiedDetails = new ArrayList<>();
    }

    public SupplyResponse(SupplyResponseStatus status, ArrayList<SupplyRequestDetail> unsatisfiedDetails, String comment) {
        this.status = status;
        this.unsatisfiedDetails = unsatisfiedDetails;
        this.comment = comment;
    }

    public SupplyResponse(SupplyResponseStatus status, ArrayList<SupplyRequestDetail> unsatisfiedDetails) {
        this.status = status;
        this.unsatisfiedDetails = unsatisfiedDetails;
        this.comment = generateComment();
    }

    public ArrayList<SupplyRequestDetail> getUnsatisfiedDetails() {
        return unsatisfiedDetails;
    }
 
    public String getComment(){
        return comment;
    }
    public SupplyResponseStatus getStatus(){
        return status;
    }
    public static String statusToString(SupplyResponseStatus status){
        return switch(status){
            case SUCCESS -> "Удовлетворен";
            case PARTIAL -> "Удовлетворен частично";
            case DENIED -> "Неудовлетворен";
            default -> "Неизвестно";
        };
    }

    final String generateComment(){
        return String.join("; ",unsatisfiedDetails.stream().map(d->d.toString()).collect(Collectors.toList()));
    }
}
