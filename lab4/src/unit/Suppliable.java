package unit;

import request.SupplyRequest;
import request.SupplyResponse;

public interface Suppliable {
    SupplyResponse makeSupplyRequest(SupplyRequest request);
}
