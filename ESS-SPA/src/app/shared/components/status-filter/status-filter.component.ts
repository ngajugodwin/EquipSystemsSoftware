import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { EntityStatus } from 'src/app/entities/models/entityStatus';

@Component({
  selector: 'app-status-filter',
  templateUrl: './status-filter.component.html',
  styleUrls: ['./status-filter.component.css']
})
export class StatusFilterComponent implements OnInit {
  @Output() statusResult = new EventEmitter<number>();

  filterStatus: { id: number; name: string }[] = [];

  constructor() {}

  ngOnInit(): void {
    this.initFilter(EntityStatus);
  }

  initFilter(status: any) {
    for (var n in status) {
      if (typeof status[n] === "number") {
        this.filterStatus.push({ id: <any>status[n], name: n });
      }
    }
  }

  onStatusChange(data: any) {
    this.statusResult.emit(Number(data.target.value));
  }
}
