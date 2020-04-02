import {Component, ElementRef, OnInit} from '@angular/core';
import {removeElementFromHtml} from "@angular/material/schematics/ng-update/upgrade-rules/hammer-gestures-v9/remove-element-from-html";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ToastrService} from "ngx-toastr";
import {PostsService} from "../../core/services/posts.service";
import {Router} from "@angular/router";
import {HttpResponse} from "@angular/common/http";

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss' ]
})
export class CreatePostComponent implements OnInit {
  markdown: any;
  createPostForm: FormGroup;
  name: any;


  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private postsService: PostsService,
    private router: Router) {
    this.createPostForm = this.fb.group(
      {
        title: [''],
        content: ['', Validators.required]
      });
  }

  ngOnInit(): void {
  }

  createPost() {
    if (this.createPostForm.invalid) {
      this.toastr.error('Нельзя создать пустой пост');
      return;
    }

    const post = this.createPostForm.value;
    this.postsService.create(post).subscribe(
      data => {
        this.toastr.success('Пост успешно создан');
        this.router.navigate(['/']);
      });
  }

}
