import { Route, Routes } from 'react-router-dom'
import { Link } from 'react-router-dom'
import { Home } from './pages/Home'
import { ItemList } from './pages/ItemList'
import { ItemLists } from './pages/ItemLists'
import { NotFound } from './pages/NotFound'
import { NewList } from './pages/NewList'
import { ItemListLayout } from './pages/ItemListLayout'
import Navbar from './Components/NavBar/Navbar'

function App() {
  return (
    <>
      <Navbar />
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/itemlists'>
          <Route index element={<ItemLists />} />
          <Route path=':id' element={<ItemList />} />
          <Route path='new' element={<NewList />} />
        </Route>

        <Route path='*' element={<NotFound />} />
      </Routes>
    </>
  )
}

export default App
