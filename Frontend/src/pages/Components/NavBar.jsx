import NavItem from "./NavItem";
import DashboardOutlinedIcon from '@mui/icons-material/DashboardOutlined';
import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import PersonOutlineOutlinedIcon from '@mui/icons-material/PersonOutlineOutlined';
import ListAltOutlinedIcon from '@mui/icons-material/ListAltOutlined';
import SettingsAccessibilityOutlinedIcon from '@mui/icons-material/SettingsAccessibilityOutlined';

export default function NavBar() {

  return (
    <nav className="pr-6">
      <ul className="menu gap-2 ">
        {/* <NavItem to="/dashboard">Dashboard</NavItem>
        <NavItem to="/dashboard/content-management">Content Management</NavItem>
        <NavItem to="/dashboard/user-management">User Management</NavItem>
        <NavItem to="/dashboard/todos">Todos</NavItem>
        <NavItem to="/dashboard/user-feedback">User Feedback</NavItem> */}
        <NavItem to="/dashboard"><DashboardOutlinedIcon /></NavItem>
        <NavItem to="/dashboard/content-management"><ArticleOutlinedIcon /></NavItem>
        <NavItem to="/dashboard/user-management"><PersonOutlineOutlinedIcon /></NavItem>
        <NavItem to="/dashboard/todos"><ListAltOutlinedIcon /></NavItem>
        <NavItem to="/dashboard/user-feedback"><SettingsAccessibilityOutlinedIcon /></NavItem>
      </ul>
    </nav>
  );
}
