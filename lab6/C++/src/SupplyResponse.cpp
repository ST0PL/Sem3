#include "SupplyResponse.hpp"
#include "SupplyRequestDetail.hpp"

SupplyResponse::SupplyResponse(const SupplyResponse& other)
    : m_status(other.m_status), m_comment(other.GetComment()), m_request(other.m_request), m_requestId(other.m_requestId) {

}

SupplyResponse::SupplyResponse(SupplyResponseStatus status, const std::string& comment, std::shared_ptr<SupplyRequest>& request)
    : m_status(status), m_comment(comment), m_request(request) {
    if (request) {
        m_requestId = (*(request)).GetId();
    }
}

SupplyResponse::SupplyResponse(SupplyResponseStatus status, std::shared_ptr<SupplyRequest>& request)
    : m_status(status) {
    if (request) {
        m_requestId = (*(request)).GetId();
        m_comment = GenerateComment(request->GetDetails());
    }
}

std::string SupplyResponse::GenerateComment(const std::vector<std::unique_ptr<SupplyRequestDetail>>& remainingDetails) {
    std::string result;

    for (size_t i = 0; i < remainingDetails.size(); ++i) {
        result += remainingDetails[i]->ToString();
        if (i + 1 < remainingDetails.size()) {
            result += "; ";
        }
    }

    return result;
}

SupplyResponseStatus SupplyResponse::GetStatus() const {
    return m_status;
}

const std::string& SupplyResponse::GetComment() const {
    return m_comment;
}

std::string SupplyResponse::StatusToString(SupplyResponseStatus status) {
    switch (status) {
    case SupplyResponseStatus::Success:
        return "Удовлетворен";
    case SupplyResponseStatus::Partial:
        return "Удовлетворен частично";
    case SupplyResponseStatus::Denied:
        return "Неудовлетворен";
    default:
        return "Неизвестно";
    }
}