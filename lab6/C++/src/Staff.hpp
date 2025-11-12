#pragma once
#include <string>
#include <memory>
#include <map>
#include "Enums.hpp"
#include "Unit.hpp"

class Staff {
public:
    Staff(const Staff&);
    Staff(int, const std::string&, Rank, Speciality);

    int GetId() const;
    const std::string& GetFullName() const;
    Rank GetRank() const;
    Speciality GetSpeciality() const;
    int GetUnitId() const;
    std::weak_ptr<Unit> GetUnit() const;

    void SetId(int);
    void SetFullName(const std::string&);
    void SetRank(Rank);
    void SetSpeciality(Speciality);
    void SetUnit(std::weak_ptr<Unit>);
    std::string ToString() const;


    static std::string RankToString(Rank rank);
    static std::string SpecialityToString(Speciality speciality);

    bool operator < (const Staff& other) const;
    bool operator > (const Staff& other) const;

    bool operator == (const Staff& other) const;
    bool operator != (const Staff& other) const;

    bool operator <= (const Staff& other) const;
    bool operator >= (const Staff& other) const;

    friend std::ostream& operator <<(std::ostream&, const Staff&);

private:
    static const std::map<Rank, std::string> m_ranks;
    static const std::map<Speciality, std::string> m_specialities;
    int m_id;
    std::string m_fullName;
    Rank m_rank;
    Speciality m_speciality;
    int m_unitId;
    std::weak_ptr<Unit> m_unit;
};