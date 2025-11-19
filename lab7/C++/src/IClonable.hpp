#pragma once
#include <memory>
class SupplyRequest;
template<typename T>
class IClonable {
public:
    IClonable() = default;
    IClonable(const IClonable&) = delete;
    virtual std::unique_ptr<T> Clone(bool = false) const = 0;
    virtual ~IClonable() = default;
};
template class IClonable<SupplyRequest>;