import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Pagination } from 'src/app/entities/models/pagination';

@Component({
  selector: 'app-pager-pagination',
  templateUrl: './pager-pagination.component.html',
  styleUrls: ['./pager-pagination.component.css']
})
export class PagerPaginationComponent implements OnInit {
  @Input() pagination: Pagination;
  @Output() nextPage = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
  }

  pageChanged(event: any) {
    this.nextPage.emit(event.page);
  }

}
