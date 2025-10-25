#include "Staff.hpp"

Staff::Staff(int id, const std::string& fullName, Rank rank, Speciality speciality) :
    m_id(id), m_fullName(fullName), m_rank(rank), m_speciality(speciality) {
}


int Staff::GetId() const {
    return m_id;
}

const std::string& Staff::GetFullName() const {
    return m_fullName;
}

Rank Staff::GetRank() const {
    return m_rank;
}

Speciality Staff::GetSpeciality() const {
    return m_speciality;
}

int Staff::GetUnitId() const {
    return m_unitId;
}

std::weak_ptr<Unit> Staff::GetUnit() const {
    return m_unit;
}

void Staff::SetId(int id) {
    m_id = id;
}

void Staff::SetFullName(const std::string& fullName) {
    m_fullName = fullName;
}

void Staff::SetRank(Rank rank) {
    m_rank = rank;
}

void Staff::SetSpeciality(Speciality speciality) {
    m_speciality = speciality;
}

void Staff::SetUnit(std::weak_ptr<Unit> unit) {
    if (auto u = unit.lock()) {
        m_unitId = u->GetId();
        m_unit = unit;
    }
}