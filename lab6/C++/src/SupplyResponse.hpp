#pragma once
#include <vector>
#include <string>
#include "Resource.hpp"
#include "Equipment.hpp"
#include "Enums.hpp"
#include "SupplyRequest.hpp"

class SupplyResponse {
public:
    SupplyResponse(SupplyResponseStatus, const std::string&, std::shared_ptr<SupplyRequest>& request);
    SupplyResponse(SupplyResponseStatus, std::shared_ptr<SupplyRequest>& request);
    SupplyResponseStatus GetStatus() const;
    const std::string& GetComment() const;
    static std::string GenerateComment(const std::vector<std::unique_ptr<SupplyRequestDetail>>&);
    static std::string StatusToString(SupplyResponseStatus);
private:
    int m_requestId;
    std::weak_ptr<const SupplyRequest> m_request;
    SupplyResponseStatus m_status;
    std::string m_comment;
};