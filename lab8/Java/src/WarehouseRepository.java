import enums.MaterialType;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Comparator;
import java.util.List;
import java.util.Optional;
import java.util.function.Predicate;
import java.util.stream.Stream;
import warehouse.WarehouseEntry;

class WarehouseRepository<T extends  WarehouseEntry<?>>{
    private final List<T> items = new ArrayList<>();
    
    public int getSize(){
        return items.size();
    }
    public boolean isEmpty(){
        return items.isEmpty();
    }
    public void clear(){
        items.clear();
    }
    public void add(T item){
        if(item != null){
            items.add(item);
        }
    }
    public void addRange(Collection<T> items){
        if(items != null){
            this.items.addAll(items);
        }
    }
    public boolean remove(int id){
        return items.removeIf(item->item.getId() == id);
    }

    public boolean removeBy(Predicate<T> predicate){
        return items.removeIf(predicate);
    }    

    public boolean removeRange(Collection<Integer> ids){
        return removeBy(item->ids.contains(item.getId()));
    }

    public boolean containsId(int id){
        return items.stream().anyMatch(item->item.getId() == id);
    }

    public Stream<T> find(Predicate<T> predicate){
        return items.stream().filter(predicate);
    }

    public Optional<T> findById(int id){
        return find(item->item.getId() == id).findFirst();
    }

    public Stream<T> findByMaterialType(MaterialType materialType){
        return find(item->item.getMaterialType() == materialType);
    }

    public Stream<T> findEmpty(){
        return find(item->item.isEmpty());      
    }

    public Stream<T> findNonEmpty(){
        return find(item->!item.isEmpty());    
    }

    public long countByMaterialType(MaterialType materialType){
        return items.stream().filter(item->item.getMaterialType() == materialType).count();
    }

    public List<T> getItems(){
        return new ArrayList<>(items);
    }

    public Stream<T> orderBy(Comparator<T> comparator) {
        return items.stream().sorted(comparator);
    }
}