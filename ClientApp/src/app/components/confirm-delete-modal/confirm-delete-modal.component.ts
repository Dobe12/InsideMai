import {Component, EventEmitter, Inject, OnInit, Output} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-confirm-delete-modal',
  templateUrl: './confirm-delete-modal.component.html',
  styleUrls: ['./confirm-delete-modal.component.scss']
})
export class ConfirmDeleteModalComponent implements OnInit {

  id: number;
  title: string;

  @Output() onDelete = new EventEmitter();

  constructor(public dialogRef: MatDialogRef<ConfirmDeleteModalComponent>,
              @Inject(MAT_DIALOG_DATA) data) {
    this.id = data.id;
    this.title = data.title;
  }

  ngOnInit(): void {
  }

  onCloseModal() {
    this.dialogRef.close();
  }
}
