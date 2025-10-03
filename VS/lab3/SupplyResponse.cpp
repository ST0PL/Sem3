#include "SupplyResponse.hpp"

SupplyResponse::SupplyResponse(SupplyResponseStatus status, std::string comment)
    : m_status(status), m_comment(comment) {
}

SupplyResponseStatus SupplyResponse::GetStatus() const {
    return m_status;
}

std::string SupplyResponse::GetComment() const {
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