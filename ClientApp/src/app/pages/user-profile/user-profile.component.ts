import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, ParamMap} from '@angular/router';
import {UsersService} from "../../core/services/users.service";
import {User} from "../../core/models/user";
import {Post} from "../../core/models/post";
import {PostsService} from "../../core/services/posts.service";
import {mergeMap} from "rxjs/operators";
import {forkJoin} from "rxjs";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  user: User;
  userPosts: Post[];

  constructor(private route: ActivatedRoute,
              private usersService: UsersService,
              private postsService: PostsService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.route.paramMap.pipe(mergeMap((param: ParamMap) => {
      const id = param.get('id');

      const user = this.usersService.get(id);
      const userPosts = this.postsService.getUserPost(id);

      return forkJoin([user, userPosts]);
    })).subscribe(result => {
      this.user = result[0] as User;
      this.userPosts = result[1] as Post[];
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
          this.user = result as User;
        });
      };
    }
  }
}
