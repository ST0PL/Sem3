package request;

import enums.SupplyResponseStatus;
import java.util.ArrayList;
import java.util.stream.Collectors;
import java.util.Map;
import static java.util.Map.entry;

public class SupplyResponse {
    SupplyResponseStatus status;
    ArrayList<SupplyRequestDetail> unsatisfiedDetails;
    String comment;

    static final Map<SupplyResponseStatus, String> statuses = Map.of(
        SupplyResponseStatus.SUCCESS, "Удовлетворен",
        SupplyResponseStatus.PARTIAL, "Удовлетворен частично",
        SupplyResponseStatus.DENIED, "Неудовлетворен");
        
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
        return statuses.getOrDefault(status, "Неизвестно");
    }

    final String generateComment(){
        return String.join("; ",unsatisfiedDetails.stream().map(d->d.toString()).collect(Collectors.toList()));
    }
}
