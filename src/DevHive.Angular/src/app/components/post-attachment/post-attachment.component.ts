import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-post-attachment',
  templateUrl: './post-attachment.component.html',
  styleUrls: ['./post-attachment.component.css']
})
export class PostAttachmentComponent implements OnInit {
  @Input() paramURL: string;
  public showFull = false;
  public fileName: string;
  public fileType: string;

  constructor()
  { }

  ngOnInit(): void {
    this.fileType = this.paramURL.includes('image') ? 'img' : 'raw';
    this.fileName = this.paramURL.match('(?<=\/)(?:.(?!\/))+$')?.pop() ?? 'Attachment';
  }

  toggleShowFull(): void {
    this.showFull = !this.showFull;
  }
}
