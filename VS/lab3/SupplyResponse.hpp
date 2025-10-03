#pragma once
#include <vector>
#include <string>
#include "Resource.hpp"
#include "Equipment.hpp"
#include "Enums.hpp"

class SupplyResponse {
public:
    SupplyResponse(SupplyResponseStatus, std::string);
    SupplyResponseStatus GetStatus() const;
    std::string GetComment() const;
    static std::string StatusToString(SupplyResponseStatus);
private:
    SupplyResponseStatus m_status;
    std::string m_comment;
};