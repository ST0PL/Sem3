#include "Staff.hpp"

Staff::Staff(const Staff& other) :
    m_id(rand()), m_fullName(other.m_fullName), m_rank(other.m_rank),
    m_speciality(other.m_speciality), m_unitId(other.m_unitId), m_unit(other.m_unit){ }

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

bool Staff::operator < (const Staff& other) const {
    return m_rank < other.m_rank;
}

bool Staff::operator > (const Staff& other) const {
    return !(*this < other);
}

bool Staff::operator == (const Staff& other) const {
    return m_id == other.m_id;
}

bool Staff::operator != (const Staff& other) const {
    return !(*this == other);
}

bool Staff::operator <= (const Staff& other) const {
    return *this < other || *this == other;
}

bool Staff::operator >= (const Staff& other) const {
    return *this > other || *this == other;
}

std::string Staff::RankToString(Rank rank) {
    return m_ranks.at(rank);
}

std::string Staff::SpecialityToString(Speciality speciality) {
    return m_specialities.at(speciality);
}

std::string Staff::ToString() const {
    std::string result;
    result.append(m_fullName).append(" [")
        .append("Звание: ").append(RankToString(m_rank))
        .append(", Специальность: ").append(SpecialityToString(m_speciality))
        .append("]");
    return result;
}

const std::map<Rank, std::string> Staff::m_ranks {
    {Rank::ePrivate, "Рядовой"},
    {Rank::eCorporal, "Ефрейтор"},
    {Rank::eJuniorSergeant, "Младший сержант"},
    {Rank::eSergeant, "Сержант"},
    {Rank::eSeniorSergeant, "Старший сержант"},
    {Rank::eSergeantMajor, "Старшина"},
    {Rank::eWarrantOfficer, "Прапорщик"},
    {Rank::eSeniorWarrantOfficer, "Старший прапорщик"},
    {Rank::eJuniorLieutenant, "Младший лейтенант"},
    {Rank::eLieutenant, "Лейтенант"},
    {Rank::eSeniorLieutenant, "Старший лейтенант"},
    {Rank::eCaptain, "Капитан"},
    {Rank::eMajor, "Майор"},
    {Rank::eLieutenantColonel, "Подполковник"},
    {Rank::eColonel, "Полковник"},
    {Rank::eMajorGeneral, "Генерал-майор"},
    {Rank::eLieutenantGeneral, "Генерал-лейтенант"},
    {Rank::eColonelGeneral, "Генерал-полковник"},
    {Rank::eArmyGeneral, "Генерал армии"}
};


const std::map<Speciality, std::string> Staff::m_specialities {
    {Speciality::eNone, "Отсутствует"},
    {Speciality::eInfantry, "Пехотинец"},
    {Speciality::eMachineGunner, "Пулеметчик"},
    {Speciality::eGrenadeLauncher, "Гранатометчик"},
    {Speciality::eReconnaissance, "Разведчик"},
    {Speciality::eSpecialForces, "Спецназ"},
    {Speciality::eSniper, "Снайпер"},
    {Speciality::eTanker, "Танкист"},
    {Speciality::eAntiAircraft, "Зенитчик"},
    {Speciality::eEngineer, "Инженер"},
    {Speciality::eMedic, "Медик"}
};