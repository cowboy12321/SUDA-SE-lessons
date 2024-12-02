#ifndef LIFE_H
#define LIFE_H
const int maxrow = 20,maxcol = 60;
class life
{
	public:
		void initialize();
		void print();
		void update();
	private:
		int grid[maxrow + 2][maxcol + 2]; 
		int neighbor_count(int row,int col);
};
#endif
