import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FeedComponent } from './components/feed/feed.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ProfileSettingsComponent } from './components/profile-settings/profile-settings.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { PostPageComponent } from './components/post-page/post-page.component';
import {AdminPanelPageComponent} from './components/admin-panel-page/admin-panel-page.component';
import {CommentPageComponent} from './components/comment-page/comment-page.component';

const routes: Routes = [
  { path: '', component: FeedComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile/:username', component: ProfileComponent },
  { path: 'profile/:username/settings', component: ProfileSettingsComponent },
  { path: 'post/:id', component: PostPageComponent },
  { path: 'comment/:id', component: CommentPageComponent },
  { path: 'admin-panel', component: AdminPanelPageComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
