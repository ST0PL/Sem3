#pragma once
#include "Unit.hpp"
#include <string>
#include <memory>
#include "Enums.hpp"

class Staff {
public:
    Staff(int, const std::string&, Rank, Speciality);

    int GetId() const;
    std::string GetFullName() const;
    Rank GetRank() const;
    Speciality GetSpeciality() const;
    int GetUnitId() const;
    std::weak_ptr<Unit> GetUnit() const;

    void SetId(int);
    void SetFullName(std::string);
    void SetRank(Rank);
    void SetSpeciality(Speciality);
    void SetUnitId(int);
    void SetUnit(std::weak_ptr<Unit>);
private:
    int m_id;
    std::string m_fullName;
    Rank m_rank;
    Speciality m_speciality;
    int m_unitId;
    std::weak_ptr<Unit> m_unit;
};