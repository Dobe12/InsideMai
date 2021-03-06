import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, ParamMap} from '@angular/router';
import {UsersService} from "../../core/services/users.service";
import {User} from "../../core/models/user";
import {Post} from "../../core/models/post";
import {PostsService} from "../../core/services/posts.service";
import {mergeMap} from "rxjs/operators";
import {forkJoin} from "rxjs";
import {ToastrService} from "ngx-toastr";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import {ChangePasswordFormComponent} from "../../components/change-password-form/change-password-form.component";
import {transition, trigger, useAnimation} from "@angular/animations";
import {bounceIn} from "ng-animate";
import {UserReactionsService} from "../../core/services/user-reactions.service";

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  animations: [
    trigger('bounceIn', [transition("userPost <=> userFav", useAnimation(bounceIn, {
      params: {
        delay: 0,
        timing: 0.3
      }
    }))])
  ]
})
export class UserProfileComponent implements OnInit {

  user: User;
  userPosts: Post[];
  userPostType = 'userPost';

  constructor(private route: ActivatedRoute,
              private usersService: UsersService,
              private postsService: PostsService,
              private toastr: ToastrService,
              private dialog: MatDialog,
              private userReactionsService: UserReactionsService) { }

  ngOnInit(): void {
    this.route.paramMap.pipe(mergeMap((param: ParamMap) => {
      const id = param.get('id');

      const user = this.usersService.get(id);
      const userPosts = this.postsService.getUserPosts(id);

      return forkJoin([user, userPosts]);
    })).subscribe(result => {
      this.user = result[0];
      this.userPosts = result[1];
    });
  }

  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]);
      reader.onload = (event) => {
        const updatedUser = this.user;
        updatedUser.userPic = event.target.result as string;

        this.usersService.update(updatedUser).subscribe(result => {
          this.toastr.success("Фотография изменена");
          this.user = result;
        });
      };
    }
  }

  switchPostType(type: string) {
    if (type === 'userFav') {
      this.postsService.getUserFavPosts(this.user.id).subscribe(
        result => {
          this.userPostType = type;
          this.userPosts = result;
        });
    } else if (type === 'userPost') {
      this.postsService.getUserPosts(this.user.id).subscribe(
        result => {
          this.userPostType = type;
          this.userPosts = result;
        });
    }
  }

  onChangePassword() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.position = {top: '200px'};
    this.dialog.open(ChangePasswordFormComponent, dialogConfig);
  }

  Subscribe(id: number) {
    this.userReactionsService.subscribe(id).subscribe(result => {
      this.user.isSubscribe = true;
      this.toastr.success("Вы успешно попдисались");
    });
  }

  Unsubscribe(id: number) {
    this.userReactionsService.unsubscribe(id).subscribe(result => {
      this.user.isSubscribe = false;
      this.toastr.success("Вы успешно отписались");
    });
  }
}
