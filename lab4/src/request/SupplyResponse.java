package request;

import enums.SupplyResponseStatus;

public class SupplyResponse {
    SupplyResponseStatus status;
    String comment;
    
    public SupplyResponse(SupplyResponseStatus status, String comment) {
        this.status = status;
        this.comment = comment;
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
    
}
