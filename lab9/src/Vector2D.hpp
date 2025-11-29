#pragma once
class Vector2D {
private:
    double m_x, m_y;

public:
    constexpr Vector2D(double x, double y) : m_x(x), m_y(y) {}

    constexpr double GetX() const
    {
        return m_x;
    }
    constexpr double GetY() const
    {
        return m_y;
    }

    constexpr void SetX(double x)
    {
        m_x = x;
    }
    constexpr void SetY(double y)
    {
        m_y = y;
    }


    constexpr double GetLength() const {
        return Sqrt(m_x * m_x + m_y * m_y);
    }

    consteval Vector2D GetNormalized() const {
        double length = GetLength();
        if (length == 0) return Vector2D(0, 0);
        return Vector2D(m_x / length, m_y / length);
    }
    constexpr double Sqrt(double x) const
    {
        return x >= 0 && x < std::numeric_limits<double>::infinity()
            ? SqrtNewtonRaphson(x, x, 0)
            : std::numeric_limits<double>::quiet_NaN();
    }
    constexpr double SqrtNewtonRaphson(double x, double curr, double prev) const
    {
        return curr == prev
            ? curr
            : SqrtNewtonRaphson(x, 0.5 * (curr + x / curr), curr);
    }

    void Print() const {
        std::cout << "(" << m_x << ", " << m_y << ")\n";
    }
};